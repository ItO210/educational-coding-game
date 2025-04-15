import { useRef } from "react";


const NewAdmin = () => {
  const token = localStorage.getItem("token");
  const addAdminForm = useRef(null);

  const addAdmin = async (evt) => {
    evt.preventDefault();
    const form = new FormData(addAdminForm.current);

    const response = await fetch(import.meta.env.VITE_API + "/addAdmin", {
      headers: { Authorization: "Bearer " + token },
      method: "POST",
      body: form,
    });

    const data = await response.json();
    if (data.message == "Admin already exists") {
      alert("Admin already exists");
    } else if (data.message == "Admin created") {
      alert("Admin created");
    }
  };
  return (
    <div className="h-screen flex flex-col w-full items-center justify-center">
      <form
        onSubmit={addAdmin}
        ref={addAdminForm}
        className="flex flex-col justify-center w-3/4 h-2/4 items-center bg-aulify-light-blue/10 rounded-lg p-4"
      >
        <h1 className="lg:my-4 text-white rounded-xl w-full p-4 text-center text-lg lg:text-3xl">
          Ingrese las Credenciales del Nuevo Administrador
        </h1>
        <input
          type="email"
          name="email"
          placeholder="Email"
          className="lg:my-4 bg-white/50 text-white placeholder-white rounded-xl w-full p-4"
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          className="my-4 bg-white/50 text-white placeholder-white rounded-xl w-full p-4"
          required
        />
        <input
          type="submit"
          value="Agregar Administrador"
          className="lg:my-4 bg-white text-blue-900 text-sm lg:text-xl rounded-3xl w-auto lg:w-2/3 p-4"
        />
      </form>
    </div>
  );
};

export default NewAdmin;
