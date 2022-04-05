using Bank_StashYourCrap.ViewModels.Base;
using System.IO;

namespace Bank_StashYourCrap.ViewModels
{
    internal class ShowBillboardWindowViewModel : BaseViewModel
    {
        public string PathToImage { get; set; }

        public ShowBillboardWindowViewModel()
        {
            var pathToDirectory = @"..\..\..\Images";
            var imageName = "Billboard.jpg";

            PathToImage = Path.Combine(pathToDirectory, imageName);
        }
    }
}
