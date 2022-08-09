using WordListsMauiHelpers.PageRouting;
using WordListsUI.Components.SideMenu.MenuField;

namespace WordListsUI.WordDataPages;

public partial class WordDataMenuItems : VerticalStackLayout
{
	public WordDataMenuItems()
	{
		InitializeComponent();
	}

	public SelectedMenuItem SelectedItem
	{
		set 
		{
			switch (value)
			{
				case SelectedMenuItem.Import:
					importDataField.IsSelected = true;
					break;
				case SelectedMenuItem.Main:
					manageDataField.IsSelected = true;
					break;
				case SelectedMenuItem.Create:
					createDataField.IsSelected = true;
					break;
				case SelectedMenuItem.Delete:
					deleteDataField.IsSelected = true;
					break;
				case SelectedMenuItem.Export:
					exportDataField.IsSelected = true;
					break;
			}
		}
	}


	private string RootRoute { get; } = PageRoutes.GetRoute(Route.WordHandling);
	private string LifetimeRoute { get; } = PageRoutes.GetRoute(Route.LifeTime);
	private string BackupRoute { get; } = PageRoutes.GetRoute(Route.Backup);


    private void ManageDataField_Loaded(object sender, EventArgs e)
	{
		if (sender is SideMenuField field)
		{
			field.TargetUIRoute = $"{RootRoute}/{nameof(WordDataPage)}";
		}
	}
	private void CreateDataField_Loaded(object sender, EventArgs e)
	{
        if (sender is SideMenuField field)
        {
            field.TargetUIRoute = $"{RootRoute}/{LifetimeRoute}/{nameof(ListGeneratorPage.ListGeneratorPage)}";
        }
    }
	private void DeleteDataField_Loaded(object sender, EventArgs e)
	{
        if (sender is SideMenuField field)
        {
            field.TargetUIRoute = $"{RootRoute}/{LifetimeRoute}/{nameof(WordCollectionEditPage.WordCollectionEditPage)}";
        }
    }
	private void ExportDataField_Loaded(object sender, EventArgs e)
	{
        if (sender is SideMenuField field)
        {
            field.TargetUIRoute = $"{RootRoute}/{BackupRoute}/{nameof(JsonExportPage.JsonExportPage)}";
        }
    }
	private void ImportDataField_Loaded(object sender, EventArgs e)
	{
        if (sender is SideMenuField field)
        {
            field.TargetUIRoute = $"{RootRoute}/{BackupRoute}/{nameof(JsonImportPage.JsonImportPage)}";
        }
    }
}