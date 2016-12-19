namespace Korrikalari.Tests

module GoogleMapsClientTests =
    open Xunit
    open Korrikalari

    [<Fact>]
    let ``get closest street from a coordinate``() =
        let getStub _ = """{
  "results": [
    {
      "address_components": [
        {
          "long_name": "9",
          "short_name": "9",
          "types": [
            "street_number"
          ]
        },
        {
          "long_name": "Albert Bridge Road",
          "short_name": "Albert Bridge Rd",
          "types": [
            "route"
          ]
        },
        {
          "long_name": "London",
          "short_name": "London",
          "types": [
            "locality",
            "political"
          ]
        }
        ],
      "formatted_address": "9 Albert Bridge Rd, London SW11 2PX, UK"
    }
  ],
  "status": "OK"
}"""
        let getClosestStreet = GoogleMapsClient.getClosestStreet getStub

        let streetName = getClosestStreet { lat= 51.473346; lon= -0.162934} 

        Assert.Equal<string>("Albert Bridge Road", streetName.Value)
