namespace WordListsViewModels.Events;

public delegate void StartWordCollectionEventHandler(object sender, TestStartEventArgs e);

public delegate void TestValidatedEventHandler(object sender, TestValidatedEventArgs e);

public delegate void ExitResultsEventHandler(object sender, EventArgs e);

public delegate void CollectionEditEventHandler(object sender, EditEventArgs e);

public delegate void CollectionEditWantedEventHandler(object sender, WordCollection collection);

public delegate void AddWantedEventHandler(object sender, EventArgs e);

public delegate void DeleteWantedEventHandler(object sender, DeleteWantedEventArgs e);

public delegate void TrainingRequestedEventHandler(object sender, StartTrainingEventArgs e);

public delegate void DBKeyDoesNotExistEventHandler(object sender, int id, string message);

public delegate void LogsCopiedEventHandler(object sender, LogsCopiedEventArgs e);

public delegate void InvalidDataEventHandler<T>(object sender, InvalidDataEventArgs<T> e);

public delegate void ParserErrorEventHandler(object sender, string error);

