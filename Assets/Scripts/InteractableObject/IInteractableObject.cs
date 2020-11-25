
//这个是所有Object的抽象类
public interface IInteractableObject
{
    void InteractObject(); //所有交互逻辑放这儿
    void Highlight(); //被玩家瞄准的时候的各种视觉表现逻辑，比如高亮啥的
    void DeHighlight();//不被玩家瞄准的时候的视觉表现逻辑

}
