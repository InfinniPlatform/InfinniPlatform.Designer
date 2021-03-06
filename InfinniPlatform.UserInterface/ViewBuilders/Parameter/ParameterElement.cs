﻿using System.Collections.Generic;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Parameter
{
    /// <summary>
    ///     Параметр представления.
    /// </summary>
    public sealed class ParameterElement : IViewChild
    {
        // Name

        private string _name;
        // Value

        private object _value;
        // View

        private View _view;
        // DataBindings

        private readonly List<ISourceDataBinding> _dataBindings
            = new List<ISourceDataBinding>();

        public ParameterElement(View view)
        {
            SetView(view);
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения.
        /// </summary>
        public ScriptDelegate OnValueChanged { get; set; }

        public View GetView()
        {
            return _view;
        }

        private void SetView(View value)
        {
            _view = value;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string value)
        {
            _name = value;
        }

        /// <summary>
        ///     Возвращает значение.
        /// </summary>
        public object GetValue()
        {
            return _value;
        }

        /// <summary>
        ///     Устанавливает значение.
        /// </summary>
        public void SetValue(object value)
        {
            if (!Equals(_value, value))
            {
                _value = value;

                NotifyDataBindings(value);
            }
        }

        public IEnumerable<ISourceDataBinding> GetDataBindings()
        {
            return _dataBindings.AsReadOnly();
        }

        public void AddDataBinding(ISourceDataBinding dataBinding)
        {
            _dataBindings.Add(dataBinding);

            dataBinding.OnSetPropertyValue += OnSetPropertyValueHandler;
        }

        public void RemoveDataBinding(ISourceDataBinding dataBinding)
        {
            if (_dataBindings.Remove(dataBinding))
            {
                dataBinding.OnSetPropertyValue -= OnSetPropertyValueHandler;
            }
        }

        private void OnSetPropertyValueHandler(dynamic context, dynamic arguments)
        {
            var propertyName = arguments.Property;
            var propertyValue = arguments.Value;

            if (string.IsNullOrEmpty(propertyName) == false)
            {
                var parameterValue = GetValue();

                if (parameterValue != null)
                {
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        _value = propertyValue;
                    }
                    else
                    {
                        ObjectHelper.SetProperty(parameterValue, propertyName, propertyValue);
                    }
                }

                NotifyDataBindings(parameterValue);
            }
        }

        private void NotifyDataBindings(dynamic parameterValue)
        {
            foreach (var dataBinding in GetDataBindings())
            {
                var propertyName = dataBinding.GetProperty();
                var propertyValue = ObjectHelper.GetProperty(parameterValue, propertyName);

                dataBinding.PropertyValueChanged(propertyValue);
            }
        }
    }
}