import { ChangeEvent, useState } from "react";

interface Props {
  length: number,
}


export const useTextField = ({ length } : Props) => {
  const [text, setText] = useState<string>("");

  const handleTextchange = (event : ChangeEvent<HTMLInputElement>) => {
    const curr = event.target.value;
    if(curr.length > length) {
      return;
    }
    setText(curr);
  }

  const clearText = () => {
    setText("");
  }

  return [text, handleTextchange, clearText] as [string, typeof handleTextchange, typeof clearText]; 
};
