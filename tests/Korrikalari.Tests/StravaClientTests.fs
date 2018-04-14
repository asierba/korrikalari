namespace Korrikalari.Tests

module StravaClientTests =
    open Xunit
    open Korrikalari.StravaClient
    open Korrikalari

    [<Fact>]
    let ``get most recent activity ``() =
        let httpGet url = 
            """
            [
                { "id": 806422995 }
            ]
            """
        let activity = getMostRecentActivity httpGet ()

        Assert.Equal(activity, { id = 806422995})

    [<Fact>]
    let ``get cooordinates for an activity``() =
        let httpGet _ =
            """
           [
                {
                "type":"latlng",
                "data":[
                    [ 51.498795, -0.141628],
                    [ 51.498765, -0.141829],
                    [ 51.49027, -0.150]],
                }
            ]
            """
        let coords = getActivityCoordinates httpGet { id = 806422995}

        let expected = [
            {lat= 51.498795; lon= -0.141628 };
            {lat= 51.498765; lon= -0.141829 };
            {lat= 51.49027; lon= -0.150}
        ] 
        Assert.Equal<seq<Coordinate>>(expected, Seq.toList coords)

    [<Fact>]
    let ``get no cooordinates for an indoor activity``() =
        let httpGet _ =
            """
           [
              {
                "type": "distance",
                "data": [
                  1.5,
                  22.8      
                ],
                "series_type": "distance",
                "original_size": 253,
                "resolution": "high"
              }
            ]
            """
        let coords = getActivityCoordinates httpGet { id = 806422995}

        let expected = [] 
        Assert.Equal<seq<Coordinate>>(expected, Seq.toList coords)

