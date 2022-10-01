namespace WordListsViewModels.Events;

public delegate void StartWordCollectionEventHandler(object sender, WordCollection collection);

public delegate void TestValidatedEventHandler(object sender, TestValidatedEventArgs e);

public delegate void ExitResultsEventHandler(object sender, EventArgs e);