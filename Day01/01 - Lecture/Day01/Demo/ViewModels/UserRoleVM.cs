using ModelsLayer;

namespace Demo.ViewModels
{
    public class UserRoleVM
    {
        public ApplicationUser User { get; set; }

        public List<string> RolesToDelete { get; set; } = new();
        public List<string> RolesToAdd { get; set; } = new();

        public List<string> RolesToDeleteNames { get; set; } = new();
        public List<string> RolesToAddNames { get; set; } = new();
    }
}
