﻿using InfinniPlatform.Core.Extensions;
using InfinniPlatform.UserInterface.Configurations;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Parameter;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LinkViews.ExistsView
{
    internal sealed class ExistsViewBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var linkView = new LinkView(context.AppView, parent, () => CreateView(context, parent, metadata));
            linkView.SetOpenMode((metadata.OpenMode as string).ToEnum(OpenMode.TabPage));

            return linkView;
        }

        private View CreateView(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            View view = null;

            // Получение метаданных представления
			var viewMetadata = ConfigResourceRepository.GetView(metadata.ConfigId, metadata.DocumentId, metadata.ViewId);

            if (viewMetadata != null)
            {
                // Создание представления по метаданным
                view = context.BuildView(parent, (object) viewMetadata);

                if (view != null)
                {
                    // Передача параметров в дочернее представление
                    BindParameters(context, parent, view, metadata.Parameters);
                }
            }

            return view;
        }

        private static void BindParameters(ObjectBuilderContext context, View parentView, View childView,
            dynamic parameters)
        {
            if (parameters != null)
            {
                foreach (var parameterLink in parameters)
                {
                    ParameterElement parameter = childView.GetParameter(parameterLink.Name);

                    if (parameter != null)
                    {
                        IElementDataBinding parameterValueBinding = context.Build(parentView, parameterLink.Value);

                        if (parameterValueBinding != null)
                        {
                            parameterValueBinding.OnPropertyValueChanged += (c, a) => parameter.SetValue(a.Value);
                            parameter.OnValueChanged += (c, a) => parameterValueBinding.SetPropertyValue(a.Value);
                        }
                    }
                }
            }
        }
    }
}