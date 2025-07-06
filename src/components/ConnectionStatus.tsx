import { Alert, Box, Button, Collapse, IconButton } from '@mui/material';
import { IconRefresh, IconWifi, IconWifiOff, IconX } from '@tabler/icons-react';
import { useEffect, useState } from 'react';

import { useLazyHealthCheckQuery } from '../redux/api';

interface ConnectionStatusProps {
  className?: string;
}

const ConnectionStatus = ({ className }: ConnectionStatusProps) => {
  const [isVisible, setIsVisible] = useState(false);
  const [lastCheckTime, setLastCheckTime] = useState<Date | null>(null);
  const [triggerHealthCheck, { isLoading, isError, isSuccess, error }] = useLazyHealthCheckQuery();

  // Periodically check API health
  useEffect(() => {
    const checkHealth = () => {
      triggerHealthCheck();
      setLastCheckTime(new Date());
    };

    // Initial check
    checkHealth();

    // Set up periodic health checks every 30 seconds
    const interval = setInterval(checkHealth, 30000);

    return () => clearInterval(interval);
  }, [triggerHealthCheck]);

  // Show/hide status based on connection state
  useEffect(() => {
    if (isError) {
      const errorStatus = (error as any)?.status;
      // If health endpoint returns 404, treat as connected (API server is responding)
      if (errorStatus === 404) {
        // Hide the status - API server is responding even if health endpoint doesn't exist
        const timer = setTimeout(() => setIsVisible(false), 2000);
        return () => clearTimeout(timer);
      } else {
        // Show error for actual connection issues
        setIsVisible(true);
      }
    } else if (isSuccess) {
      // Hide after successful connection, but with a delay
      const timer = setTimeout(() => setIsVisible(false), 3000);
      return () => clearTimeout(timer);
    }
  }, [isError, isSuccess, error]);

  const handleRetry = () => {
    triggerHealthCheck();
    setLastCheckTime(new Date());
  };

  const handleClose = () => {
    setIsVisible(false);
  };

  const getStatusColor = () => {
    if (isLoading) return 'info';
    if (isError) return 'error';
    if (isSuccess) return 'success';
    return 'info';
  };

  const getStatusMessage = () => {
    if (isLoading) return 'Checking API connection...';
    if (isError) {
      const errorStatus = (error as any)?.status;
      if (errorStatus === 404) {
        return 'API server is running (health endpoint not available)';
      }
      if (errorStatus === 'FETCH_ERROR' || errorStatus === 'TIMEOUT_ERROR') {
        return 'Unable to connect to the API server. Please check your connection.';
      }
      return 'API connection error. Some features may not work properly.';
    }
    if (isSuccess) return 'Connected to API server';
    return 'Checking connection...';
  };

  const getStatusIcon = () => {
    if (isLoading) return <IconRefresh className="animate-spin" size={20} />;
    if (isError) return <IconWifiOff size={20} />;
    if (isSuccess) return <IconWifi size={20} />;
    return <IconWifi size={20} />;
  };

  return (
    <Box className={className}>
      <Collapse in={isVisible}>
        <Alert
          severity={getStatusColor()}
          icon={getStatusIcon()}
          action={
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              {isError && (
                <Button color="inherit" size="small" onClick={handleRetry} disabled={isLoading} startIcon={<IconRefresh size={16} />}>
                  Retry
                </Button>
              )}
              <IconButton size="small" onClick={handleClose} color="inherit">
                <IconX size={16} />
              </IconButton>
            </Box>
          }
          sx={{ mb: 1 }}>
          <Box>
            <Box component="span" sx={{ fontWeight: 'medium' }}>
              {getStatusMessage()}
            </Box>
            {lastCheckTime && (
              <Box component="div" sx={{ fontSize: '0.75rem', opacity: 0.8, mt: 0.5 }}>
                Last checked: {lastCheckTime.toLocaleTimeString()}
              </Box>
            )}
          </Box>
        </Alert>
      </Collapse>
    </Box>
  );
};

export default ConnectionStatus;
