using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicShare.Models;

public enum SendCategory
{
    Files,
    Images,
    Music,
    Videos,
    Contacts,
    Archive
}


public record SendCategoryItem(string Title, string Icon,SendCategory SendCategory);
