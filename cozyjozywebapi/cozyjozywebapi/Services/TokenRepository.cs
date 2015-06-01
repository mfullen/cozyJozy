using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using cozyjozywebapi.Infrastructure.Core;

namespace cozyjozywebapi.Services
{
    public enum TokenType
    {
        PasswordReset
    }
    public class Token
    {
        public int Id { get; set; }
        public string TokenCode { get; set; }
        public TokenType TokenType { get; set; }
        public string UserId { get; set; }
    }
    public class InMemoryTokenRepository : ITokenRepository
    {
        private readonly ISet<Token> _tokens = new HashSet<Token>();

        public Token Add(Token entity)
        {
            _tokens.Add(entity);
            return entity;
        }

        public Token Update(Token entity, Func<Token, int> getKey)
        {
            throw new NotImplementedException();
        }

        public Token Delete(Token entity)
        {
            var token = _tokens.Where(t => t.TokenCode == entity.TokenCode)
                .Where(t => t.TokenType == entity.TokenType)
                .FirstOrDefault(t => t.UserId == entity.UserId);
            if (token == null)
                return null;
            _tokens.Remove(token);
            return token;
        }

        public IQueryable<Token> Delete(Expression<Func<Token, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public Token GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Token GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Token> All()
        {
            return _tokens.AsQueryable();
        }

        public IQueryable<Token> Where(Expression<Func<Token, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public Task<List<Token>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Token> FindAsync(Expression<Func<Token, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Task<List<Token>> FindAllAsync(Expression<Func<Token, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Token Find(string userId, TokenType tokenType)
        {
            var token = _tokens.Where(t => t.UserId == userId).FirstOrDefault(t => t.TokenType == tokenType);
            return token;
        }

        public Token Find(string userId, string code)
        {
            var token = _tokens.Where(t => t.UserId == userId).FirstOrDefault(t => t.TokenCode == code);
            return token;
        }
    }

    public interface ITokenRepository : IRepository<Token>
    {
        Token Find(string userId, TokenType tokenType);
        Token Find(string userId, string code);
    }
}
