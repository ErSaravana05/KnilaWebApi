using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnilaWebApi.Model
{
    [Table("Book")]
    public class BookModel
    {
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorFirstName { get; set; }
        public decimal Price { get; set; }
        [Key]
        public int SNo { get; set;}
        // Property for MLA citation
        public string MlaCitation
        {
            get
            {
                return $"{AuthorLastName}, {AuthorFirstName}. \"{Title}.\" {Publisher}.";
            }
        }

        // Property for Chicago citation
        public string ChicagoCitation
        {
            get
            {
                return $"{AuthorLastName}, {AuthorFirstName}. {Title}. {Publisher}.";
            }
        }
    }
}
