using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.Tests;

public class TestSampleProvider : ISampleProvider
{
    public string[] GetSamples()
    {
        return
        [
            "Great Toy, hard to find! We get ours online here or shipped from a friend in America!Our little Papillion-Yorkie mix loves it. Every night I am in the back garden kicking the ball for him! He destroys tennis balls by chewing off the fluff and our wee dog finds them to big to carry/catch. We stumbled across this in our local petstore and our dog was hooked! Sadly, local pet store no longer imports them.Stars all around for this one!",
            "I have bought several of these small Orbee Tuff balls, because my dog simple loves them! The orange ball does glow in the dark that makes it easy to find. I have nothing but good things to say about these small balls. The only down side is Trix, my dog, keeps loosing them!",
            "It is a quality ball but the small is still too big and heavy for my little yorkie to play with comfortably.",
            "I gave it 5 stars because my little dog had so much fun riping it apart but only took him 24 hrs to do it buy that is 23 hrs longer than other balls",
            "I've used many products to try and help the water clarity, but only kent marine was capable of successfully clearing the water every time. Caution though, adding it to water will drop the pH. Mix with tank water on the side and check pH before placing it in the tank."
        ];
    }
}