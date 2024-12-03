public interface Destroyable{
    public int health {get;set;}
    public void ApplyDamage(int damage);
    public void Destroy();
}