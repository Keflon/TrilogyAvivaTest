using System.Threading.Tasks;

namespace TrilogyAvivaTest.Services.Alert
{
	public class AlertService : IAlertService
	{
		public AlertService()
		{
		}
		public async Task DisplayAlertAsync(string title, string message, string cancel)
		{
			await App.Current.MainPage.DisplayAlert(title, message, cancel);
		}

		public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
		{
			return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}
	}
}
