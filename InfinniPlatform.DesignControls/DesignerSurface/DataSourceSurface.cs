﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using InfinniPlatform.DesignControls.Controls.DataSources;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.DesignerSurface
{
    public partial class DataSourceSurface : UserControl, ILayoutProvider
    {
        private readonly List<DataSourceObject> _dataSources = new List<DataSourceObject>();

        public DataSourceSurface()
        {
            InitializeComponent();

            gridBinding.DataSource = DataSources;
        }

        public List<DataSourceObject> DataSources
        {
            get { return _dataSources; }
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public void SetLayout(dynamic value)
        {
        }

        public string GetPropertyName()
        {
            return "DataSources";
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new List<dynamic>();
            foreach (ILayoutProvider dataSourceObject in DataSources)
            {
                dynamic instance = new DynamicWrapper();
                dynamic layout = dataSourceObject.GetLayout();
                ObjectHelper.SetProperty(instance, dataSourceObject.GetPropertyName(), layout);
                instanceLayout.Add(instance);
            }
            return instanceLayout;
        }

        public void Clear()
        {
            _dataSources.Clear();
            GridControlDataSources.RefreshDataSource();
        }

        private void repositoryItemButtonEditSource_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            var dataSource =
                DataSources.ElementAt(GridViewDataSources.FocusedRowHandle).DataSource as IPropertiesProvider;
            if (dataSource != null)
            {
                var form = new PropertiesForm();

                var validationRules = dataSource.GetValidationRules();
                form.SetValidationRules(validationRules);
                var propertyEditors = dataSource.GetPropertyEditors();
                form.SetPropertyEditors(propertyEditors);
                var simpleProperties = dataSource.GetSimpleProperties();
                form.SetSimpleProperties(simpleProperties);
                var collectionProperties = dataSource.GetCollections();
                form.SetCollectionProperties(collectionProperties);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    dataSource.ApplySimpleProperties();
                    dataSource.ApplyCollections();
                    GridViewDataSources.HideEditor();
                    GridViewDataSources.RefreshData();
                }
                else
                {
                    form.RevertChanges();
                }
            }
        }

        private void AddDocumentDataSourceButton_Click(object sender, EventArgs e)
        {
            var dataSourceObject = new DataSourceObject();
            DataSources.Add(dataSourceObject);

            var documentDataSource = new DocumentDataSource();
            dataSourceObject.DataSource = documentDataSource;
            documentDataSource.ObjectInspector = ObjectInspector;
            GridViewDataSources.RefreshData();
        }

        private void AddObjectDataSourceButton_Click(object sender, EventArgs e)
        {
            var dataSourceObject = new DataSourceObject();
            DataSources.Add(dataSourceObject);

            var objectDataSource = new ObjectDataSource();
            dataSourceObject.DataSource = objectDataSource;
            objectDataSource.ObjectInspector = ObjectInspector;
            GridViewDataSources.RefreshData();
        }

        private void DeleteDataSourceButton_Click(object sender, EventArgs e)
        {
            if (GridViewDataSources.FocusedRowHandle >= 0)
            {
                DataSources.RemoveAt(GridViewDataSources.FocusedRowHandle);
                GridViewDataSources.RefreshData();
            }
        }

        private void GetLayoutButton_Click(object sender, EventArgs e)
        {
            dynamic value = GetLayout();

            var valueEdit = new ValueEdit();
            valueEdit.Value = value.ToString();
            valueEdit.ShowDialog();
        }

        private void SetLayoutButton_Click(object sender, EventArgs e)
        {
            var valueEdit = new ValueEdit();
            if (valueEdit.ShowDialog() == DialogResult.OK)
            {
                ProcessJson(valueEdit.Value);
            }
        }

        public void ProcessJson(dynamic dataSources)
        {
            DataSources.Clear();
            foreach (var source in dataSources)
            {
                var dataSourceObject = new DataSourceObject();
                if (source.DocumentDataSource != null)
                {
                    var documentDataSource = new DocumentDataSource();
                    documentDataSource.ObjectInspector = ObjectInspector;
                    documentDataSource.LoadProperties(source.DocumentDataSource);
                    dataSourceObject.DataSource = documentDataSource;
                }
                else if (source.ObjectDataSource != null)
                {
                    var objectDataSource = new ObjectDataSource();
                    objectDataSource.ObjectInspector = ObjectInspector;
                    objectDataSource.LoadProperties(source.ObjectDataSource);
                    dataSourceObject.DataSource = objectDataSource;
                }
                DataSources.Add(dataSourceObject);
            }
            GridViewDataSources.RefreshData();
        }

        private void GridViewDataSources_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            var value = (DataSourceObject) GridViewDataSources.GetRow(e.RowHandle);
            if (value == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(value.DataSourceName))
            {
                e.DisplayText = "<DataSource name not specified>";
            }
            else
            {
                e.DisplayText = value.DataSourceName;
            }
        }
    }
}