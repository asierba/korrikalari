namespace Korrikalari.Tests

module GoogleMapsClientShould =
    open Xunit
    open Korrikalari
    open Korrikalari.GoogleMapsClient

    let aCoordinate =  { lat= 51.473346; lon= -0.162934}
  
    [<Fact>]
    let ``get street name from address component of type route``() =
        let httpGet url = """
{
  "results": [
    {
      "address_components": [
          {
            "long_name": "9",
            "types": [ "street_number" ]
          },
          {
            "long_name": "Albert Bridge Road",
            "short_name": "Albert Bridge Rd",
            "types": [ "route" ]
          },
          {
            "long_name": "London",
            "types": [ "locality", "political" ]
          }
          ],
      }
    ],
  "status": "OK"
}"""
        let getClosestStreet = GoogleMapsClient.getClosestStreet httpGet

        let streetName = getClosestStreet aCoordinate

        Assert.Equal<StreetName>(Some "Albert Bridge Road", streetName)

    [<Fact>]
    let ``get street name from first result``() =
        let httpGet url = """
{
  "results": [
    {
      "address_components": [
          {
            "long_name": "Albert Bridge Road",
            "types": [ "route" ]
          }
          ],
    },
    {
      "address_components": [
          {
            "long_name": "London Bridge Road",
            "types": [ "route" ]
          }
          ],
      }
    ],
  "status": "OK"
}"""
        let getClosestStreet = GoogleMapsClient.getClosestStreet httpGet

        let streetName = getClosestStreet aCoordinate

        Assert.Equal<StreetName>(Some "Albert Bridge Road", streetName)


    [<Fact>]
    let ``get no street name when no results``() =
      let httpGet url = """
          {
            "results" : [],
            "status" : "ZERO_RESULTS"
          }"""
      let getClosestStreet = GoogleMapsClient.getClosestStreet httpGet

      let streetName = getClosestStreet aCoordinate

      Assert.Equal<StreetName>(None, streetName)
    
 