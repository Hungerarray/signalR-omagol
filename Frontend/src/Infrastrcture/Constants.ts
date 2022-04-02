export const USERNAME_LIMIT = 32;
export const TEXTMESSAGE_LIMIT = 200;


const test = import.meta.env.VITE_BACKEND;
console.log(test);
const temp = "http://localhost:8000";
export const app = test ?? "http://localhost:5230"