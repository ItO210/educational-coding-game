import LineGraph from "./Graphs/LineGraph";
import BarGraph from "./Graphs/BarGraph";
import { useEffect, useState } from "react";
import PieGraph from "./Graphs/PieGraph";
import { useNavigate } from "react-router-dom";

const Dashboard = () => {
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const [isLoading, setLoading] = useState(true);

  const [playersByLevel, setPlayersByLevel] = useState([]);
  const [sesionsByDay, setSessionsByDay] = useState([]);
  const [userCount, setUserCount] = useState([]);
  const [playerCount, setPlayerCount] = useState([]);
  const [timePlayed, setTimePlayed] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const urls = [
          import.meta.env.VITE_API + "/playersByLevel",
          import.meta.env.VITE_API + "/sessionsByDay",
          import.meta.env.VITE_API + "/countUsers",
          import.meta.env.VITE_API + "/countPlayers",
          import.meta.env.VITE_API + "/timePlayedAll",
        ];

        const requests = urls.map((url) =>
          fetch(url, {
            headers: { Authorization: "Bearer " + token },
          }).then((response) => {
            if (!response.ok) {
              throw new Error("HTTP error, status = " + response.status);
            }
            return response.json();
          })
        );

        const [
          playersByLevel,
          sessionsByDay,
          countUsers,
          countPlayers,
          timePlayed,
        ] = await Promise.all(requests);

        setPlayersByLevel(playersByLevel);
        setSessionsByDay(sessionsByDay);
        setUserCount(countUsers);
        setPlayerCount(countPlayers);
        setTimePlayed(timePlayed);
        setLoading(false);
      } catch (error) {
        console.log("Error fetching data:", error);
        localStorage.removeItem("token");
        navigate("/login", { replace: true });
      }
    };

    fetchData();
  }, []);

  return (
    <div className="h-auto lg:h-full grid grid-rows-2 lg:grid-rows-3">
      <div className="h-auto row-span-1 grid grid-cols-1 lg:grid-cols-3">
        <div className="h-96 lg:h-auto col-span-1 m-2 rounded-lg bg-aulify-light-blue/10 lg:col-span-1">
          <div className="w-full h-full flex flex-col items-center justify-center">
            <h1 className="text-2xl font-bold my-2 text-white">
              Jugadores Totales
            </h1>
            <span className="text-4xl font-bold my-2 text-aulify-yellow">
              {isLoading ? "" : playerCount[0].players}
            </span>
          </div>
        </div>
        <div className="h-96 lg:h-auto col-span-1 m-2 rounded-lg lg:ml-2 bg-aulify-light-blue/10 lg:col-span-1">
          <LineGraph
            title={"Tiempo Por Día"}
            chartData={timePlayed}
            dataKey={"day"}
            dataValueKey={"time_played"}
            color={"#F6BA27"}
          />
        </div>
        <div className="h-96 lg:h-auto col-span-1 lg:col-span-1 m-2 rounded-lg bg-aulify-light-blue/10">
          <PieGraph
            title="Usuarios Aulify"
            chartData={userCount}
            dataKey={"count"}
            dataValueKey={"type"}
            color="#F6BA27"
          />
        </div>
      </div>
      <div className="h-auto row-span-1 lg:row-span-2 grid grid-cols-1 lg:grid-cols-2">
        <div className="h-auto rounded-lg m-2 bg-aulify-light-blue/10">
          <BarGraph
            title={"Jugadores Por Nivel"}
            chartData={playersByLevel}
            dataKey={"id_level"}
            dataValueKey={"player_count"}
            color={"#F6BA27"}
          />
        </div>
        <div className="h-auto rounded-lg m-2 bg-aulify-light-blue/10">
          <LineGraph
            title={"Sesiones Por Día"}
            chartData={sesionsByDay}
            dataKey={"day"}
            dataValueKey={"count"}
            color={"#F6BA27"}
          />
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
