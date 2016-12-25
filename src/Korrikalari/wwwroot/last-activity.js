var LastActivity = React.createClass({
    getInitialState: function() {
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open( "GET", '/api/activity/last', false );
        xmlHttp.send( null );
        var json = JSON.parse(xmlHttp.responseText);
        return {
            id : json.activityId,
            streets: json.streets
        }
    },
    render: function() {
        return (
            <div>
                <h1>Last Acitivity in Strava</h1>
                <h2>Id: {this.state.id}</h2>

                <h3>Streets</h3>
                <ol>
                    { 
                        this.state.streets.map((item) => 
                            <li key={item}>{item}</li>)
                    }
                </ol>
            </div>
        );
    }
});

ReactDOM.render(
    <LastActivity/>,
    document.getElementById('main')
);