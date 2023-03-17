namespace Shop
{
    public partial class PointOfIssue
    {
        public string Adress
        {
            get
            {
                if (Build != null)
                {
                    return "г. " + City + ", ул. " + Street + ", д. " + Build;
                }

                return "г. " + City + ", ул. " + Street;
            }
        }
    }
}