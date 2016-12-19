using Database.DatabaseDataSetTableAdapters;

namespace Database
{
    public class DatabaseUsers
    {
        private static readonly DatabaseUsers Instance = new DatabaseUsers();

        public DatabaseUsers()
        {
            Adapter = new UsersTableAdapter();
        }

        public UsersTableAdapter Adapter { get; }

        public static DatabaseDataSet.UsersRow GetByDeveloperId(string developerId)
        {
            var table = Instance.Adapter.GetDataBy(developerId);
            return table[0];
        }

        public static DatabaseDataSet.UsersRow GetUserAuth(int userAuth)
        {
            return null;
        }
    }
}
