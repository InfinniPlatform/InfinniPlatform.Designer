﻿using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class ScenarioContainerNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.Scenario + "Container";

        private static readonly string[] ElementChildrenTypes =
        {
            MetadataType.Scenario
        };

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            elementNode.ElementName = Resources.ScenarioContainer;

            elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode, MetadataType.Scenario);
            elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
            elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
            elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
            elementNode.RegisterPasteCommand(builder);

            FactoryHelper.AddEmptyElement(elements, elementNode);
        }
    }
}