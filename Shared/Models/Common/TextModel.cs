﻿using EyE.Shared.Enums;
using EyE.Shared.Models.Common.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace EyE.Shared.Models.Common
{
    public class TextModel : IDbFolderItem, IEquatable<TextModel>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public FolderNames FolderName { get; set; }
        [JsonIgnore] public bool IsEditing { get; set; }

        public bool Equals(TextModel other) => Id == other.Id;
        public override bool Equals(object obj) => Equals(obj as TextModel);
        public override int GetHashCode() => Id;
    }
}
