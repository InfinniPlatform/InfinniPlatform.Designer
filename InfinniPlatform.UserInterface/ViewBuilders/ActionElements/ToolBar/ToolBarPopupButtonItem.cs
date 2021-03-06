﻿using System.Collections.Generic;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Mvvm;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar
{
    /// <summary>
    ///     Элемент панели инструментов в виде кнопки со всплывающим окном.
    /// </summary>
    public sealed class ToolBarPopupButtonItem : ToolBarItem<BarSubItem>
    {
        // Action

        private BaseAction _action;
        // Image

        private string _image;
        // Items

        private readonly ToolBarItemCollection _items;

        public ToolBarPopupButtonItem(View view)
            : base(view)
        {
            _items = new ToolBarItemCollection(Control);

            Control.Command = new DelegateCommand(OnClickToolBarButton);
        }

        // OnClick

        /// <summary>
        ///     Возвращает или устанавливает обработчик события нажатия на кнопку.
        /// </summary>
        public ScriptDelegate OnClick { get; set; }

        private void OnClickToolBarButton()
        {
            if (Control.Links != null && Control.Links.Count > 0)
            {
                var link = Control.Links[0] as BarSubItemLink;

                if (link != null)
                {
                    var linkControl = link.LinkControl as BarSubItemLinkControl;

                    if (linkControl != null)
                    {
                        linkControl.ShowPopup();
                    }
                }
            }

            this.InvokeScript(OnClick);

            var action = GetAction();

            if (action != null)
            {
                action.Execute();
            }
        }

        /// <summary>
        ///     Возвращает изображение кнопки.
        /// </summary>
        public string GetImage()
        {
            return _image;
        }

        /// <summary>
        ///     Устанавливает изображение кнопки.
        /// </summary>
        public void SetImage(string value)
        {
            _image = value;

            Control.Glyph = ImageRepository.GetImage(value);
        }

        /// <summary>
        ///     Возвращает действие при нажатии на кнопку.
        /// </summary>
        public BaseAction GetAction()
        {
            return _action;
        }

        /// <summary>
        ///     Устанавливает действие при нажатии на кнопку.
        /// </summary>
        public void SetAction(BaseAction value)
        {
            _action = value;
        }

        /// <summary>
        ///     Добавляет элемент в список.
        /// </summary>
        public void AddItem(IToolBarItem item)
        {
            _items.AddItem(item);
        }

        /// <summary>
        ///     Удаляет элемент из списка.
        /// </summary>
        public void RemoveItem(IToolBarItem item)
        {
            _items.RemoveItem(item);
        }

        /// <summary>
        ///     Возвращает элемент по имени.
        /// </summary>
        public IToolBarItem GetItem(string name)
        {
            return _items.GetItem(name);
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable<IToolBarItem> GetItems()
        {
            return _items.GetItems();
        }

        // Click

        /// <summary>
        ///     Осуществляет программное нажатие на кнопку.
        /// </summary>
        public void Click()
        {
            Control.PerformClick();
        }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            return GetItems();
        }
    }
}