namespace API.DTOs
{
    public class PageRequest
    {
        public PageRequest(int page, int pageSize)
        {
            this.page = page;
            this.pageSize = pageSize;
        }

        public int page { get; set; }
        public int pageSize{ get; set; }
    }
}
