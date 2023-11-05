namespace WordListsViewModels;
public class TrainingConfigViewModel : ITrainingConfigViewModel
{
    // Use query prop in view to call async func that gets and parses the wordlist from database
    // Wordlist will be built whilst the user chooses which buttons to press
    // IsBusy is set to true while build is on (if user tries to press continue, app will wait for build to be ready)

    // StartTrainingPage => TrainingConfigPage => (some)TrainingPage
}
