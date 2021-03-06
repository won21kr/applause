using System.Collections;
using System.Collections.Generic;
using Applause.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Tasks;
using ItemisApp.DataAccessLayer;
using ItemisApp.Model;
using ItemisApp.Views;

namespace ItemisApp.ViewModel
{
    public class BlogDetailsViewModel : ViewModelBase
    {
    	
		public BlogDetailsViewModel()
		{
			if (IsInDesignMode)
			{
			}
			else {
				LoadData();
			}
		}
		
		#region Data Source
		private void LoadData()
		{
    	
		}

        public void AcceptData(object result) 
        {
            if (result is BlogItem) 
            {
                BlogItem = (BlogItem)result;
                MapSections(result);
                
            }
        }
        
        private void MapSections(object result)
        {
			var grouped = new List<IEnumerable>();
			Sections = grouped;        
        }
        
		#endregion
		
		#region BlogItem Property
		public const string BlogItemPropertyName = "BlogItem";
		private BlogItem _blogItem = null;
		public BlogItem BlogItem
		{
			get 
			{ 
				return _blogItem;
			}
			
			set
			{
				if (_blogItem == value)
				{
					return;
				}
				_blogItem = value;
				RaisePropertyChanged(BlogItemPropertyName);
			}
		}
		#endregion
		
		#region Sections Property
        public const string SectionsProperyName = "Sections";
        List<IEnumerable> _sections = null;

        public List<IEnumerable> Sections
        {
            get
            {
                return _sections;
            }
            set
            {
                if (_sections == value)
                {
                    return;
                }

                var oldValue = _sections;
                _sections = value;

                RaisePropertyChanged(SectionsProperyName);
            }
        }
		#endregion
		
		#region Navigation
        public RelayCommand<object> NavigationCommand
        {
            get
            {
                return new RelayCommand<object>(
                	(selection) =>
                	{
					
						if (selection is Contact)
						{
							NavigationMessage msg = new NavigationMessage()
							{
								PageName = "PersonDetailsPage",
								ViewModelName = "PersonDetailsViewModel"
							};
							msg.Payload.Add("contact", (Contact)selection);
							Messenger.Default.Send<NavigationMessage>(msg);
						}

						if (selection is string)
						{
							string url = (string)selection;
                        	if (url.IndexOf("tel:") == 0)
                        	{
                            	PhoneCallTask task = new PhoneCallTask();
                            	task.PhoneNumber = url.Substring("tel:".Length);
                            	task.Show();
                        	}
                        	else if (url.IndexOf("mailto:") == 0)
                        	{
                            	EmailComposeTask task = new EmailComposeTask();
                            	task.To = url.Substring("mailto:".Length);
                            	task.Show();
                        	}
                        	else
                        	{
                        		// open browser
                        	}
						}
                	}
				);
            }
        }
		#endregion
    }
}
