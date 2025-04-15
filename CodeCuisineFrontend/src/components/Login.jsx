import { useRef } from "react";
import { useNavigate } from "react-router-dom";
import logo from "../assets/Logo.png";

const Login = () => {
  const loginForm = useRef(null);
  const navigate = useNavigate();

  const login = async (evt) => {
    evt.preventDefault();
    const form = new FormData(loginForm.current);

    const response = await fetch(import.meta.env.VITE_API + "/loginAdmin", {
      method: "POST",
      body: form,
    });

    const data = await response.json();

    if (data.token) {
      localStorage.setItem("token", data.token);
      navigate("/dashboard");
    } else {
      alert("incorrect user or password");
    }
  };

  return (
    <div className="bg-gradient-to-b from-red-400 to-red-600 flex flex-col w-full h-screen items-center justify-center">
      <img src={logo} alt="" className="w-1/4" />
      <form
        onSubmit={login}
        ref={loginForm}
        className="py-6 flex flex-col justify-center w-1/2 items-center"
      >
        <input
          type="email"
          name="email"
          placeholder="Email"
          className="my-4 bg-white/50 placeholder-red-900 rounded-xl w-full p-4"
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Contraseña"
          className="my-4 bg-white/50 placeholder-red-900 rounded-xl w-full p-4"
          required
        />
        <input
          type="submit"
          value="Iniciar"
          className="my-4 bg-white text-gray-900 rounded-3xl w-1/2 p-4"
        />
      </form>
    </div>
  );
};

export default Login;
