using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.Interfaces
{
    interface IUpdatableUI
    {
        void UpdateInterfaceText(Text textToUpdate, string content);
    }
}
