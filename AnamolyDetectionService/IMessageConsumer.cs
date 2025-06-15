namespace AnamolyDetectionService;

public interface IMessageConsumer<T>
{
   void startConsuming();
  
   void stopConsuming();
   
   event EventHandler<T> onMessageReceived;
   
   
}