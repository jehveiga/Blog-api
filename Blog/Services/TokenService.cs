using Blog.Extensions;
using Blog.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); // Objeto que fará a manipulação do token
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);   // Obtendo a chave da classe estatica criada de Configuration e convertendo para um array de bytes,
                                                                       // a chave informada será usada para encriptar e decriptar.
            var claims = user.GetClaims();

            var tokenDescriptor = new SecurityTokenDescriptor // Objeto que ficará todos as informações vinculado ao token
            {
                Subject = new ClaimsIdentity(claims), // Adicionar uma lista de Claims vinculado ao usuário para ser passado para configuração de criação do token
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials( // Criará o token encriptado passando a chave e o algoritmo que será usado
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); // Criando um novo token baseado nas informações passadas no objeto acima

            return tokenHandler.WriteToken(token);
        }
    }
}
