﻿using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ProcessDesigner
{
    internal sealed class ProcessDesignerElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var editor = new ProcessDesignerElement(parent);
            editor.ApplyElementMeatadata((object) metadata);

            // Привязка к источнику данных

            IElementDataBinding valueBinding = context.Build(parent, metadata.Value);

            if (valueBinding != null)
            {
                valueBinding.OnPropertyValueChanged += (c, a) => editor.SetValue(a.Value);
                editor.OnValueChanged += (c, a) => valueBinding.SetPropertyValue(a.Value, force: true);

                var sourceValueBinding = valueBinding as ISourceDataBinding;

                if (sourceValueBinding != null)
                {
                    // Передача контекста редактору

                    var dataSourceName = sourceValueBinding.GetDataSource();
                    var dataSource = parent.GetDataSource(dataSourceName);

                    editor.SetConfigId(dataSource.GetConfigId);
                    editor.SetDocumentId(dataSource.GetDocumentId);
                    editor.SetVersion(dataSource.GetVersion);
                }
            }

            return editor;
        }
    }
}