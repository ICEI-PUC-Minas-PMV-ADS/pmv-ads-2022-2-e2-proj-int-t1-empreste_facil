﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresteFacil.Models.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 255 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o e-mail.")]
        [DisplayName("Email*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha o telefone celular.")]
        [DisplayName("Telefone celular*")]
        [DataType(DataType.PhoneNumber)]
        public string Celular { get; set; }

        [DisplayName("Telefone fixo")]
        [DataType(DataType.PhoneNumber)]
        public string TelefoneFixo { get; set; }

        [Display(Name = "Senha*")]
        [Required(ErrorMessage = "Insira uma senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public Perfil Perfil { get; set; }
    }

    public enum Perfil 
    {
        Pessoa_Fisica,
        Pessoa_Juridica
    }
}
