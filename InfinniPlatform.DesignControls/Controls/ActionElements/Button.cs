﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.Controls.ActionElements
{
    public partial class Button : UserControl, IPropertiesProvider, ILayoutProvider, IClientHeightProvider,
        IInspectedItem
    {
        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public Button()
        {
            InitializeComponent();

            InitProperties();
        }

        public int GetClientHeight()
        {
            return 32;
        }

        public bool IsFixedHeight()
        {
            return true;
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public dynamic GetLayout()
        {
            var instanceLayout = new DynamicWrapper();
            DesignerExtensions.SetSimplePropertiesToInstance(_simpleProperties, instanceLayout);
            return instanceLayout;
        }

        public void SetLayout(dynamic value)
        {
            //no inner layout
        }

        public string GetPropertyName()
        {
            return "Button";
        }

        public void ApplySimpleProperties()
        {
            ButtonElement.Text = _simpleProperties["Text"].Value.ToString();
            ButtonElement.Name = _simpleProperties["Name"].Value.ToString();
        }

        public void ApplyCollections()
        {
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _simpleProperties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return new Dictionary<string, CollectionProperty>();
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_simpleProperties, value);
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>().InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators(
                    "Button");
        }

        private void InitProperties()
        {
            _simpleProperties.InheritBaseElementSimpleProperties();
        }
    }
}