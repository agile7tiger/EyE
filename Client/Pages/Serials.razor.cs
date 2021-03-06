﻿using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("Serials/{StrFolderName}")]
    public partial class Serials
    {
        protected override async Task OnInitializedAsync()
        {
            await InitializeAsync("api/Serials");
        }
    }
}
