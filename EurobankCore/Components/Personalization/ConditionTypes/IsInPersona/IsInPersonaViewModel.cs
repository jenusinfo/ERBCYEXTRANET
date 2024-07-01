using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eurobank.Personalization
{
    public class IsInPersonaViewModel
    {
        [Required]
        [Display(Name = "Persona Code Name")]
        public string PersonaCodeName { get; set; }


        public List<IsInPersonaListItemViewModel> AllPersonas { get; set; }
    }
}