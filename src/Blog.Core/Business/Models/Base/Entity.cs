namespace Blog.Core.Business.Models.Base
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Ativo {  get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

    }
}
