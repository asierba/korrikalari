namespace Korrikalari.Tests

module GoogleMapsClientTests =
    open Xunit
    open Korrikalari

    [<Fact>]
    let ``get closest street from a coordinate``() =
        let getStub _ = """{
  "results": [
    {
      "formatted_address": "9 Albert Bridge Rd, London SW11 2PX, UK"
    }
  ],
  "status": "OK"
}"""
        let getClosestStreet = GoogleMapsClient.getClosestStreet getStub

        let streetName = getClosestStreet { lat= 51.473346; lon= -0.162934} 

        Assert.Equal<string>("9 Albert Bridge Rd, London SW11 2PX, UK", streetName.Value)
