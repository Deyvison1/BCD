using System.Collections.Generic;

namespace BCD.WebApi.Dtos
{
    public class PapelDto
    {
        public int Id { get; set; }        
        public string Setor { get; set; }
        public List<PapelDto> Usuarios { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
                    
                    

    }
}