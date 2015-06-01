using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        private static readonly object CacheLockObject = new object();
        //private readonly ISet<Token> _tokens = new HashSet<Token>();
        private string CreateCacheKey(Token entity)
        {
            return CreateCacheKey(entity.UserId, entity.TokenType.ToString());
        }

        private string CreateCacheKey(string userId, string tokenType)
        {
            var cacheKey = string.Join("|", new[] { userId, tokenType });
            return cacheKey;
        }

        protected Token InsertIntoCache(string cacheKey, Token entity)
        {
            lock (CacheLockObject)
            {
                HttpRuntime.Cache.Insert(cacheKey, entity, null, DateTime.Now.AddHours(24), TimeSpan.Zero);
            }

            return entity;
        }

        public Token Add(Token entity)
        {
            // _tokens.Add(entity);

            InsertIntoCache(CreateCacheKey(entity), entity);
            return entity;
        }

        public Token Update(Token entity, Func<Token, int> getKey)
        {
            throw new NotImplementedException();
        }

        public Token Delete(Token entity)
        {
            var removal = HttpRuntime.Cache.Remove(CreateCacheKey(entity));

            return entity;
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
            throw new NotImplementedException();
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
            var result = HttpRuntime.Cache[CreateCacheKey(userId, tokenType.ToString())] as Token;

            return result;
        }

        public Token Find(string userId, string code)
        {
            var result = HttpRuntime.Cache[CreateCacheKey(userId, TokenType.PasswordReset.ToString())] as Token;
            if (result != null && result.TokenCode == code)
            {
                return result;
            }
            return null;
        }
    }

    public interface ITokenRepository : IRepository<Token>
    {
        Token Find(string userId, TokenType tokenType);
        Token Find(string userId, string code);
    }
}
