using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_StashYourCrap.Models;

namespace Bank_StashYourCrap.Bank.Services.Modifiers
{
    internal class ConfidentialDataHider
    {
        public ClientModel HideData(ClientModel clientModel)
        {
            var passSeries = Hide(clientModel.PassSeries.ToString());
            var passNumber = Hide(clientModel.PassNumber.ToString());
            clientModel.PassSeries = passSeries;
            clientModel.PassNumber = passNumber;
            return clientModel;
        }

        private string Hide(string line)
        {
            var newLine = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                newLine.Append('*');
            }
            return newLine.ToString();
        }
    }
}
