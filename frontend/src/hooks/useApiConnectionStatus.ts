import { useEffect, useState } from 'react';
import { useLazyHealthCheckQuery } from '../redux/api';

interface ApiConnectionStatus {
  isConnected: boolean;
  isChecking: boolean;
  lastError: any;
  lastCheckTime: Date | null;
  retryConnection: () => void;
}

/**
 * Custom hook to monitor API connection status
 * @param checkInterval - Interval in milliseconds to check connection (default: 30000ms = 30 seconds)
 * @returns Connection status object with state and retry function
 */
export const useApiConnectionStatus = (checkInterval: number = 30000): ApiConnectionStatus => {
  const [lastCheckTime, setLastCheckTime] = useState<Date | null>(null);
  const [triggerHealthCheck, { isLoading, isError, isSuccess, error }] = useLazyHealthCheckQuery();

  // Periodic health check
  useEffect(() => {
    const checkHealth = () => {
      triggerHealthCheck();
      setLastCheckTime(new Date());
    };

    // Initial check
    checkHealth();

    // Set up periodic checks
    const interval = setInterval(checkHealth, checkInterval);

    return () => clearInterval(interval);
  }, [triggerHealthCheck, checkInterval]);

  const retryConnection = () => {
    triggerHealthCheck();
    setLastCheckTime(new Date());
  };

  // Determine connection status
  const isConnected = isSuccess || (isError && (error as any)?.status === 404);

  return {
    isConnected,
    isChecking: isLoading,
    lastError: error,
    lastCheckTime,
    retryConnection,
  };
};

export default useApiConnectionStatus; 