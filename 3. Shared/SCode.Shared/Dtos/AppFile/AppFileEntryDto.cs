// ReSharper disable All
namespace SCode.Shared.Dtos.AppFile
{
    public class AppFileEntryDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string Name { get; set; }
        
        public bool IsDirectory { get; set; }

        #region Equality Comparers

        protected bool Equals(AppFileEntryDto other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AppFileEntryDto) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(AppFileEntryDto left, AppFileEntryDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AppFileEntryDto left, AppFileEntryDto right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
