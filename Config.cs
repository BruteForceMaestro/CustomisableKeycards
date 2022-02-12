using Interactables.Interobjects.DoorUtils;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomisableKeycards
{
    public class Config : IConfig
    {
        [Description("Indicates if plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Keycards that are not included in this will have default permissions.")]
        public Dictionary<ItemType, KeycardPermissions> KeycardPermission { get; set; } = new Dictionary<ItemType, KeycardPermissions>()
        {
            [ItemType.KeycardContainmentEngineer] = KeycardPermissions.AlphaWarhead | KeycardPermissions.Checkpoints | KeycardPermissions.ContainmentLevelThree | KeycardPermissions.Intercom
        };
    }
}