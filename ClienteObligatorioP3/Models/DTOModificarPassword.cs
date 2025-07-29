using System.ComponentModel.DataAnnotations;

namespace ClienteObligatorioP3.Models
{
    public class DTOModificarPassword
    {
        public string PasswordOriginal { get; set; }
        public string PasswordNueva { get; set; }
    }

}
