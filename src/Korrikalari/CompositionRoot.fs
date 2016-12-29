namespace Korrikalari

open Korrikalari.FileSystemCache

module CompositionRoot =
    let getClosestStreet = GoogleMapsClient.getClosestStreet (cache WebClient.get)
    let getMostRecentActivity = 
        StravaClient.getMostRecentActivity (WebClient.getWithHeaders [StravaClient.authHeader])
    let getActivityCoordinates =
        StravaClient.getActivityCoordinates (cache (WebClient.getWithHeaders [StravaClient.authHeader]))
