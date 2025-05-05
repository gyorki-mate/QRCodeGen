// src/App.tsx
import React, { useState } from "react";
import axios from "axios";
// src/main.tsx or src/index.tsx
import './index.css';


function App() {
  const [data, setData] = useState("");
  const [qrCode, setQrCode] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleGenerate = async () => {
    setLoading(true);
    setQrCode(null);
    setError("");

    try {
      const response = await axios.post("http://localhost:5197/api/qr/generate-qrcode", {
        data: data.trim(),
      }, {
        responseType: 'blob',
      });

      if (!response.status || response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`);
      }
      const blob = await response.data;
      const imageUrl = URL.createObjectURL(blob);
      setQrCode(imageUrl);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (err.response) {
          // Server responded with an error
          setError(`Server Error: ${err.response.status} - ${err.response.statusText}`);
        } else if (err.request) {
          // Request was made but no response
          setError("No response from server. Is it running?");
        } else {
          // Something happened setting up the request
          setError(`Request error: ${err.message}`);
        }
      } else {
        // Non-Axios error
        setError(`Unexpected error: ${err instanceof Error ? err.message : String(err)}`);
      }
    } finally {
      setLoading(false);
    }
  };

  return (<div className="flex justify-center items-center h-screen bg-blue-700">
      <div className="bg-white shadow-xl rounded-2xl p-8 max-w-md w-full">
        <h1 className="text-2xl font-bold text-center text-gray-800 mb-6">
          QR Code Generator
        </h1>
        <textarea
          value={data}
          onChange={(e) => setData(e.target.value)}
          placeholder="Enter text or URL to generate QR code"
          className="w-full p-3 border rounded-lg mb-4 resize-none h-28"
        />
        <button
          onClick={handleGenerate}
          disabled={loading || !data.trim()}
          className={`w-full py-3 rounded-lg text-white font-semibold transition ${loading || !data.trim() ? "bg-gray-400 cursor-not-allowed" : "bg-blue-600 hover:bg-blue-700"}`}
        >
          {loading ? "Generating..." : "Generate QR Code"}
        </button>

        {error && <p className="text-red-600 mt-4 text-center">{error}</p>}

        {qrCode && (<div className="mt-6 flex flex-col items-center">
            <img
              src={qrCode}
              alt="Generated QR Code"
              className="max-w-96 max-h-96 border rounded-lg"
            />
            <a
              href={qrCode}
              autoSave="asd.png"
              className="mt-4 text-sm text-blue-600 hover:underline"
            >
              Download QR Code
            </a>
          </div>)}
      </div>
    </div>);
}

export default App;
