

namespace Olechka
{
    public interface I_damage
    {
        public Health Main_health { get;}

        /// <summary>
        /// �������� ����
        /// </summary>
        /// <param name="_damage">���������� �����</param>
        /// <param name="_killer">��� ���� (����� null)</param>
        public virtual void Add_Damage(int _damage, Character_abstract _killer)
        {
            Add_damage(_damage, _killer);
        }

        protected abstract void Add_damage(int _damage, Character_abstract _killer);
    }
}