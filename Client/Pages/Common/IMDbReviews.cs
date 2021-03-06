﻿using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EyE.Client.Pages.Common
{
    public class IMDbReviews<T> : Reviews<T> where T: IMDbModel, new()
    {
        public override async Task InitializeAsync(string pageURI, bool needUpdateTempItems = true)
        {
            SortingParameters.Add(SortingKeys.IMDbRating, SortingKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRating, FilterKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRatingMore, FilterKeys.IMDbRatingMore.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRatingLess, FilterKeys.IMDbRatingLess.GetAttribute<DisplayAttribute>().Name);

            await base.InitializeAsync(pageURI);
        }

        public override async Task AddItemIfNotExistAsync()
        {
            if (!await UserChecker.CheckAdminRoleAsync() || !await UserChecker.CheckNullOrWhiteSpaceAsync(ItemAdderViewModel.Id))
                return;

            var model = await IMDbHelper.GetIMDbModelAsync<T>(ItemAdderViewModel.Id, PublicClient);
            model.FolderName = FolderName;
            await AddItemIfNotExistAsync((T)model);
        }

        public async Task UpdateItemAsync(T oldItem)
        {
            if (!await UserChecker.CheckAdminRoleAsync())
                return;

            var newItem = await IMDbHelper.GetIMDbModelAsync<T>(oldItem.Link, PublicClient);
            newItem.Id = oldItem.Id;
            newItem.FolderName = oldItem.FolderName;
            newItem.AddingDate = oldItem.AddingDate;
            await UpdateItemAsync(oldItem, (T)newItem);
        }
    }
}
