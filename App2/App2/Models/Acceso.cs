using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    class Acceso
    {
        public string Centro;
        public string Tipo;
        public Persona persona;
        public DateTime fecha;

        public Acceso() {
            Persona persona = new Persona();
        }
    }

}
