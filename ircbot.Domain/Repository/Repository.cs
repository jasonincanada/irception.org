namespace ircbot.Domain
{
    public partial class Repository
    {
        private ircbotEntities _context;

        public Repository()
        {
            _context = new ircbotEntities();
        }
        
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
